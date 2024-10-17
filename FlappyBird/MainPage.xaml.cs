namespace FlappyBird;

public partial class MainPage : ContentPage
{
    const int Gravidade = 3;
	const int tempoEntreFrames = 25;
	const int forcaPulo = 30;
	const int maxTempoPulando = 3; //frames
	const int aberturaMinima = 100;
	bool EstaMorto = true;
	bool estaPulando = false;
	double largura_Janela = 0;
	double altura_Janela = 0;
	int velocidade = 20;
	int tempoPulando = 0;
	int score = 0;
	public MainPage()
	{
		InitializeComponent();
	}
    void AplicaGravidade()
	{
		galinha.TranslationY+= Gravidade;
	}
	
	async Task Desenha()
	{
		while(!EstaMorto)
		{
			AplicaGravidade();
			GerenciaTronco();
			if (VerificaColisao())
			{
				EstaMorto = true;
				frameGameOver.IsVisible = true;
				labelGameOver.Text="Você passou \n por "+ score + " \n troncos";
				break;
			}
			if (estaPulando)
			   AplicaPulo();
			   else
			   AplicaGravidade();

			await Task.Delay(tempoEntreFrames);
		}
	}
	void OnGameOverCliked(object s, TappedEventArgs a)
	{
		frameGameOver.IsVisible = false;
		Inicializar();
		Desenha();
	}
	void Inicializar()
	{
		galinha.TranslationY = 0;
		EstaMorto = false;
		score = 0;
	}
    protected override void OnSizeAllocated(double w, double h)
    {
        base.OnSizeAllocated(w, h);
		largura_Janela = w;
        altura_Janela = h;
    }
	void GerenciaTronco()
	{
     TroncoCima.TranslationX -= velocidade;
	 TroncoBaixo.TranslationX -= velocidade;
	 if (TroncoBaixo.TranslationX <-largura_Janela)
	 {
		TroncoBaixo.TranslationX = 0;
		TroncoCima.TranslationX = 0;
		var alturaMax = -100;
		var alturaMin = -TroncoBaixo.HeightRequest;
		TroncoCima.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
		TroncoBaixo.TranslationY=TroncoCima.TranslationY+aberturaMinima+TroncoBaixo.HeightRequest;
		score ++;
		labelScore.Text = "Troncos:" + score.ToString ("D3");
	 }
	}
	bool VerificaColisao()
	{
		if(!EstaMorto)
		{
			if(VerificaColisaoTeto() || VerificaColisaoChao() )
			{
				return true;
			}
		}
		return false;
	}
	bool VerificaColisaoTeto()
	{
		var minY = -altura_Janela/2;
		if (galinha.TranslationY <= minY)
		   return true;
		else
		   return false;
	}
	bool VerificaColisaoChao()
	{
		var maxY = altura_Janela/2 - Chao.HeightRequest - 65;
		if (galinha.TranslationY >= maxY)
		   return true;
		else
		   return false;
	}
	void AplicaPulo()
	{
		galinha.TranslationY -= forcaPulo;
		tempoPulando++;

		if (tempoPulando >= maxTempoPulando)
		{
			estaPulando = false;
			tempoPulando = 0;
		}
	}

	void OnGridClicked(object s, TappedEventArgs args)
	{
		estaPulando = true;
	}

}
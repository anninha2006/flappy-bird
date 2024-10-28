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
		{
			TroncoCima.TranslationX =- largura_Janela;
			TroncoBaixo.TranslationY =- largura_Janela;
			galinha.TranslationX = 0;
			galinha.TranslationY = 0;
			score = 0;
			GerenciaTronco();
		}
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
		score ++;
		if (score % 2 == 0)
		velocidade++;
	 }
	}
	bool VerificaColisao()
	{
		if (! EstaMorto)
		{
			return VerificaColisaoTeto() || VerificaColisaoChao() || VerificaColisaoTroncoCima() || VerificaColisaoTroncoBaixo();
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

	bool VerificaColisaoTroncoCima()
  {
    var posicaoHorizontalPardal = (largura_Janela - 50) - (galinha.WidthRequest / 2);
    var posicaoVerticalPardal   = (altura_Janela / 2) - (galinha.HeightRequest / 2) + galinha.TranslationY;

    if (
         posicaoHorizontalPardal >= Math.Abs(TroncoCima.TranslationX) - TroncoCima.WidthRequest &&
         posicaoHorizontalPardal <= Math.Abs(TroncoCima.TranslationX) + TroncoCima.WidthRequest &&
         posicaoVerticalPardal   <= TroncoCima.HeightRequest + TroncoCima.TranslationY
       )
      return true;
    else
      return false;
  }
	bool VerificaColisaoTroncoBaixo()
	{
		var posicaoHPardal = (largura_Janela / 2) - (galinha.WidthRequest / 2);
		var posicaoVPardal = (altura_Janela / 2) - (galinha.HeightRequest / 2) + galinha.TranslationY;

		if (posicaoHPardal >= Math.Abs(TroncoBaixo.TranslationX) + TroncoBaixo.WidthRequest && 
		posicaoHPardal <= Math.Abs(TroncoBaixo.TranslationX) + TroncoBaixo.WidthRequest && 
		posicaoVPardal <= TroncoBaixo.HeightRequest + TroncoBaixo.TranslationY)
			return true;
		else
			return false;
	}
}
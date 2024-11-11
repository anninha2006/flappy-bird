namespace FlappyBird;

public partial class MainPage : ContentPage
{
    const int Gravidade = 5;
	const int tempoEntreFrames = 25;
	const int forcaPulo = 30;
	const int maxTempoPulando = 3; //frames
	const int aberturaMinima = 80;
	bool EstaMorto = true;
	bool estaPulando = false;
	double largura_Janela = 0;
	double altura_Janela = 0;
	int velocidade = 5;
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
			if (estaPulando)
			   AplicaPulo();
			   else
			   AplicaGravidade();

			GerenciaTronco();

			if (VerificaColisao())
			{
				EstaMorto = true;
				frameGameOver.IsVisible = true;
				labelGameOver.Text= $"Você passou por \n " + score + " troncos";
				break;
			}

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
		EstaMorto = false;
		score = 0;
	    TroncoCima.TranslationX =- largura_Janela;
	    TroncoBaixo.TranslationX =- largura_Janela;
		galinha.TranslationY = 0;
		galinha.TranslationX = 0;
		GerenciaTronco();
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

		var alturaMax = -(TroncoBaixo.HeightRequest * 0.1);
		var alturaMin = -(TroncoBaixo.HeightRequest * 0.8);

		TroncoCima.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
		TroncoBaixo.TranslationY=TroncoCima.TranslationY+aberturaMinima+TroncoBaixo.HeightRequest;
	
		labelScore.Text = "Troncos:" + score.ToString ("D3");
		score ++;
		if (score % 2 == 0)
		velocidade++;
	 }
	}
	bool VerificaColisao()
	
		{
			return (!EstaMorto && (VerificaColisaoTeto() || VerificaColisaoChao() || VerificaColisaoTronco()));
		}      

    bool VerificaColisaoTronco()
  {
    if (VerificaColisaoTroncoBaixo() || VerificaColisaoTroncoCima())
      return true;
    else
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
    var posicaoHorizontalGalinha = largura_Janela - 50 - (galinha.WidthRequest / 2);
    var posicaoVerticalGalinha = (altura_Janela / 2) - (galinha.HeightRequest / 2) + galinha.TranslationY;

    if (posicaoHorizontalGalinha >= Math.Abs(TroncoCima.TranslationX) - TroncoCima.WidthRequest &&
        posicaoHorizontalGalinha <= Math.Abs(TroncoCima.TranslationX) + TroncoCima.WidthRequest &&
        posicaoVerticalGalinha   <= TroncoCima.HeightRequest + TroncoCima.TranslationY)
      return true;
    else
      return false;
  }
	bool VerificaColisaoTroncoBaixo()
	{
		var posicaoHorizontalGalinha = largura_Janela - 50 - galinha.WidthRequest / 2;
		var posicaoVerticalGalinha = (altura_Janela / 2) - (galinha.HeightRequest / 2) + galinha.TranslationY;

        var yMaxTronco = TroncoCima.HeightRequest + TroncoCima.TranslationY + aberturaMinima;

		if (posicaoHorizontalGalinha >= Math.Abs(TroncoBaixo.TranslationX) - TroncoCima.WidthRequest && 
			posicaoHorizontalGalinha <= Math.Abs(TroncoBaixo.TranslationX) + TroncoCima.WidthRequest && 
			posicaoVerticalGalinha >= yMaxTronco)
			return true;
		else
			return false;
		
	}
}
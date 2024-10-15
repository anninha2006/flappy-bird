namespace FlappyBird;

public partial class MainPage : ContentPage
{
    const int Gravidade = 1;
	const int tempoEntreFrames = 25;
	bool EstaMorto = true;
	double largura_Janela = 0;
	double altura_Janela = 0;
	int velocidade = 20;
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
		galinha.TranslationY = 0;
		EstaMorto = false;
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
}
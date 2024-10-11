namespace FlappyBird;

public partial class MainPage : ContentPage
{
    const int Gravidade = 1;
	const int tempoEntreFrames = 25;
	bool EstaMorto = false;
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
	while (!EstaMorto)
	{
		AplicaGravidade();
		await Task.Delay(tempoEntreFrames);
		GerenciaTronco();
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
		EstaMorto=false;
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

}


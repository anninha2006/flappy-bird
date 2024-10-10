namespace FlappyBird;

public partial class MainPage : ContentPage
{
    const int Gravidade = 1;
	const int tempoEntreFrames = 25;
	bool EstaMorto = false;
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
	
}


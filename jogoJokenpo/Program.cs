
public static class Program
{
    static int Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        IniciarJogo();


        Console.WriteLine("Bem vindo ao jogo de Jokenpo!");


        bool jogoEmAndamento = true;
        while (jogoEmAndamento)
        {
            Console.WriteLine("Escolha entre: 0(Pedra), 1(Papel) ou 2(Tesoura) ou pressione q para sair!");

            string? escolhaUsuario = Console.ReadLine()?.Trim();
            int escolhaConsole = -1;

            if (escolhaUsuario is null || string.Equals(escolhaUsuario, "q", StringComparison.OrdinalIgnoreCase))
            {
                jogoEmAndamento = false;
                continue;
            }
            else if (!int.TryParse(escolhaUsuario, out escolhaConsole) || escolhaConsole < 0 || escolhaConsole > 2)
            {
                Console.Error.WriteLine("Opção inválida!");
                continue;
            }

            int opcaoMaquina = Random.Shared.Next(Enum.GetValues<PecasJogo>().Length);

            OpcoesPlacar resultado = RegrasJogo.CalcularResultado((PecasJogo)escolhaConsole, (PecasJogo)opcaoMaquina);

            switch (resultado)
            {
                case OpcoesPlacar.Vitoria:
                    Console.WriteLine("Vitória!");
                    break;
                case OpcoesPlacar.Derrota:
                    Console.WriteLine("Derrota!");
                    break;
                case OpcoesPlacar.Empate:
                    Console.WriteLine("Empate!");
                    break;
                default:
                    Console.WriteLine("Resultado não esperado");
                    break;
            }

            Console.WriteLine($"Sua escolha: {(PecasJogo)escolhaConsole}, Escolha da máquina: {(PecasJogo)opcaoMaquina}");

        }

        Console.WriteLine("Obrigado por jogar!");
        return 0;

    }

    private static void IniciarJogo()
    {
        Console.WriteLine("Iniciando o jogo de Jokenpo...");
        if (Console.IsInputRedirected)
            return;

        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }
}

public enum PecasJogo
{
    Pedra,
    Papel,
    Tesoura
}

public enum OpcoesPlacar
{
    Vitoria,
    Derrota,
    Empate
}

public static class RegrasJogo
{
    public static OpcoesPlacar CalcularResultado(PecasJogo usuario, PecasJogo maquina)
    {
        if (!Enum.IsDefined(usuario))
            throw new ArgumentOutOfRangeException(nameof(usuario), usuario, "Jogada inválida.");
        if (!Enum.IsDefined(maquina))
            throw new ArgumentOutOfRangeException(nameof(maquina), maquina, "Jogada inválida.");

        if (usuario == maquina)
            return OpcoesPlacar.Empate;

        return (usuario, maquina) switch
        {
            (PecasJogo.Pedra, PecasJogo.Tesoura) => OpcoesPlacar.Vitoria,
            (PecasJogo.Tesoura, PecasJogo.Papel) => OpcoesPlacar.Vitoria,
            (PecasJogo.Papel, PecasJogo.Pedra) => OpcoesPlacar.Vitoria,
            _ => OpcoesPlacar.Derrota
        };
    }
}

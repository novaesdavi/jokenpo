Console.WriteLine("Bem vindo ao jogo de Jokenpo!");


bool jogoEmAndamento = true;
while (jogoEmAndamento)
{
    Console.WriteLine("Escolha entre: 0(Pedra), 1(Papel) ou 2(Tesoura) ou pressione q para sair!");

    string? escolhaUsuario = Console.ReadLine();
    int escolhaConsole = -1;

    if (escolhaUsuario == "q")
    {
        jogoEmAndamento = false;
        continue;
    }
    else if (!int.TryParse(escolhaUsuario, out escolhaConsole) || escolhaConsole < 0 || escolhaConsole > 2)
    {
        Console.WriteLine("Opção inválida!");
        continue;
    }

    int opcaoMaquina = new Random().Next(0, 2);

    OpcoesPlacar resultado = RegrasJogo.ValidarJogoMaquinaVenceu((PecasJogo)escolhaConsole, (PecasJogo)opcaoMaquina);

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
    public static OpcoesPlacar ValidarJogoMaquinaVenceu(PecasJogo usuario, PecasJogo maquina)
    {

        if (usuario == maquina)
            return OpcoesPlacar.Empate;

        if (usuario == PecasJogo.Pedra && maquina == PecasJogo.Tesoura)
            return OpcoesPlacar.Vitoria;

        if (usuario == PecasJogo.Pedra && maquina == PecasJogo.Papel)
            return OpcoesPlacar.Derrota;

        if (usuario == PecasJogo.Tesoura && maquina == PecasJogo.Papel)
            return OpcoesPlacar.Vitoria;

        return OpcoesPlacar.Derrota;


    }
}
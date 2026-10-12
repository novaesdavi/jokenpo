namespace jogoJokenpo.Tests;

public class RegrasJogoTests
{
    [Theory]
    [InlineData(PecasJogo.Pedra)]
    [InlineData(PecasJogo.Papel)]
    [InlineData(PecasJogo.Tesoura)]
    public void Jogadas_iguais_retornam_Empate(PecasJogo peca)
    {
        Assert.Equal(OpcoesPlacar.Empate, RegrasJogo.CalcularResultado(peca, peca));
    }

    [Theory]
    [InlineData(PecasJogo.Pedra, PecasJogo.Tesoura)]
    [InlineData(PecasJogo.Tesoura, PecasJogo.Papel)]
    [InlineData(PecasJogo.Papel, PecasJogo.Pedra)]
    public void Usuario_com_jogada_vencedora_retorna_Vitoria(PecasJogo usuario, PecasJogo maquina)
    {
        Assert.Equal(OpcoesPlacar.Vitoria, RegrasJogo.CalcularResultado(usuario, maquina));
    }

    [Theory]
    [InlineData(PecasJogo.Tesoura, PecasJogo.Pedra)]
    [InlineData(PecasJogo.Papel, PecasJogo.Tesoura)]
    [InlineData(PecasJogo.Pedra, PecasJogo.Papel)]
    public void Usuario_com_jogada_perdedora_retorna_Derrota(PecasJogo usuario, PecasJogo maquina)
    {
        Assert.Equal(OpcoesPlacar.Derrota, RegrasJogo.CalcularResultado(usuario, maquina));
    }

    [Theory]
    [InlineData((PecasJogo)5, PecasJogo.Pedra)]
    [InlineData(PecasJogo.Pedra, (PecasJogo)(-1))]
    public void Jogada_fora_do_enum_lanca_excecao(PecasJogo usuario, PecasJogo maquina)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RegrasJogo.CalcularResultado(usuario, maquina));
    }
}

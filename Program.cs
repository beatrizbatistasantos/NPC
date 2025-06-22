using System.Security.Cryptography;

const int estado_procurando = 0;
const int estado_atacando = 1;
const int estado_fugindo = 2;
const int estado_morto = 3;

const double delay = 2.0;

int estado = estado_procurando;
int estadoAnterior = estado_procurando;
int transicoes = 0;
string descricaoEstado = "";
string descricaoEstadoAnterior = "";

bool inimigoProximo = false;
bool ferido = false;
bool eliminado = false;

Console.WriteLine("--- Simulação de IA de NPC ---\n");
Console.WriteLine("Produzido por: Bia Batista, Bia Giovanna e Laís Campos");

while (estado != estado_morto)
{
    estadoAnterior = estado;
    transicoes++;

    SimularEventos(estado);
    estado = DefinirProximoEstado(estado);

    descricaoEstadoAnterior = ObterDescricao(estadoAnterior);
    descricaoEstado = ObterDescricao(estado);

    Console.WriteLine($"-- #{transicoes,3} {descricaoEstadoAnterior,10}: Ferido = {(ferido ? "S" : "N")}, InimigoProximo = {(inimigoProximo ? "S" : "N")}, Eliminado = {(eliminado ? "S" : "N")} => {descricaoEstado}");
    Thread.Sleep((int)(1000 / delay));
}

Console.WriteLine($"\nO NPC sobreviveu por {transicoes - 1} transições.");
Console.WriteLine("Pressione qualquer tecla para sair...");
Console.ReadKey();

// Funções auxiliares

void SimularEventos(int estadoAtual)
{
    switch (estadoAtual)
    {
        case estado_procurando:
            if (RandomNumberGenerator.GetInt32(0, 2) == 0) ferido = false;
            if (RandomNumberGenerator.GetInt32(0, 2) == 0) inimigoProximo = true;
            break;

        case estado_atacando:
            if (RandomNumberGenerator.GetInt32(0, 2) == 0)
            {
                ferido = true;
                if (RandomNumberGenerator.GetInt32(0, 2) == 0) eliminado = true;
            }
            if (RandomNumberGenerator.GetInt32(0, 2) == 0) inimigoProximo = false;
            break;

        case estado_fugindo:
            if (RandomNumberGenerator.GetInt32(0, 4) == 0) eliminado = true;
            if (RandomNumberGenerator.GetInt32(0, 4) == 0) ferido = false;
            if (RandomNumberGenerator.GetInt32(0, 2) == 0) inimigoProximo = false;
            break;
    }
}

int DefinirProximoEstado(int estadoAtual)
{
    if (eliminado) return estado_morto;

    return estadoAtual switch
    {
        estado_procurando => (!ferido && inimigoProximo) ? estado_atacando : estado_procurando,
        estado_atacando => ferido ? estado_fugindo : (!inimigoProximo ? estado_procurando : estado_atacando),
        estado_fugindo => ferido ? estado_fugindo : estado_procurando,
        _ => estado_morto
    };
}

string ObterDescricao(int estado)
{
    return estado switch
    {
        estado_procurando => "Procurando",
        estado_atacando => "Atacando",
        estado_fugindo => "Fugindo",
        estado_morto => "Morto",
        _ => "Desconhecido"
    };
}

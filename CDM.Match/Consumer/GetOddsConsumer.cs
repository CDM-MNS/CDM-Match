using CDM.Match.DTO;
using MassTransit;

namespace CDM.Match.Consumer;

public class GetOddsConsumer : IConsumer<GetOddsResponseDTO>
{
    public GetOddsConsumer()
    {
    }
    
    public Task Consume(ConsumeContext<GetOddsResponseDTO> context)
    {
        Console.WriteLine(context.CorrelationId);
        Console.WriteLine(context.Message.OddSecondTeamWinning);
        Console.WriteLine(context.Message.OddFirstTeamWinning);
        Console.WriteLine(context.Message.OddDraw);
        Console.WriteLine(context.Headers);
        return Task.CompletedTask;
    }
}
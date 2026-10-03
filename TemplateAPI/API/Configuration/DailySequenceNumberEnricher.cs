using Serilog.Core;
using Serilog.Events;

namespace API.Configuration;

public sealed class DailySequenceNumberEnricher(TimeProvider? timeProvider = null) : ILogEventEnricher
{
    #region Data Members

    private readonly object       lockObject   = new();
    private readonly TimeProvider timeProvider = timeProvider ?? TimeProvider.System;

    private DateOnly currentDate;
    private long     currentSequenceNumber;

    #endregion


    #region Methods

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        long sequenceNumber;

        lock (lockObject)
        {
            DateOnly today = DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime);

            if (today != currentDate)
            {
                currentDate = today;
                currentSequenceNumber = 0;
            }

            sequenceNumber = ++currentSequenceNumber;
        }

        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("SequenceNumber", sequenceNumber));
    }

    #endregion
}

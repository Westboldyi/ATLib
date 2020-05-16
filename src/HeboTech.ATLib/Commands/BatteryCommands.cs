﻿using HeboTech.ATLib.Parsers;
using HeboTech.ATLib.Results;
using HeboTech.ATLib.States;
using HeboTech.MessageReader;
using System.Threading;
using System.Threading.Tasks;

namespace HeboTech.ATLib.Commands
{
    public static class BatteryCommands
    {
        public static async ValueTask<ATResult<BatteryStatusResult>> GetBatteryStatusAsync(this ICommunicator<string> comm, CancellationToken cancellationToken = default)
        {
            await comm.Write("AT+CBC\r\n", cancellationToken);
            var message = await comm.ReadSingleMessageAsync(Constants.BYTE_LF, cancellationToken);
            if (BatteryStatusParser.TryParse(message, ResponseFormat.Numeric, out ATResult<BatteryStatusResult> batteryResult))
            {
                message = await comm.ReadSingleMessageAsync(Constants.BYTE_LF, cancellationToken);
                if (OkParser.TryParse(message, ResponseFormat.Numeric, out ATResult<OkResult> _))
                    return batteryResult;
                else if (ErrorParser.TryParse(message, ResponseFormat.Numeric, out ATResult<ErrorResult> errorResult))
                    return ATResult.Error<BatteryStatusResult>(errorResult.ErrorMessage);
            }
            return ATResult.Error<BatteryStatusResult>(batteryResult.ErrorMessage);
        }
    }
}
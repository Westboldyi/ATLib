﻿using HeboTech.ATLib.Communication;
using HeboTech.ATLib.Parsers;
using HeboTech.ATLib.Results;
using HeboTech.ATLib.States;
using System.Threading;
using System.Threading.Tasks;

namespace HeboTech.ATLib.Commands._3GPP_TS_27_007
{
    public static class PinCommands
    {
        public static async ValueTask<ATResult<PinStatusResult>> GetPinStatusAsync(
            this ICommunicator comm,
            ResponseFormat responseFormat,
            CancellationToken cancellationToken = default)
        {
            await comm.Write($"AT+CPIN?\r", cancellationToken);
            var message = await comm.ReadLineAsync(cancellationToken);
            if (PinStatusParser.TryParse(message, responseFormat, out ATResult<PinStatusResult> pinResult))
            {
                message = await comm.ReadLineAsync(cancellationToken);
                if (OkParser.TryParse(message, responseFormat, out ATResult<OkResult> _))
                    return pinResult;
                else if (ErrorParser.TryParse(message, responseFormat, out ATResult<ErrorResult> errorResult))
                    return ATResult.Error<PinStatusResult>(errorResult.ErrorMessage);
            }
            else if (ErrorParser.TryParse(message, responseFormat, out ATResult<ErrorResult> errorResult))
                return ATResult.Error<PinStatusResult>(errorResult.ErrorMessage);
            return ATResult.Error<PinStatusResult>(Constants.PARSING_FAILED);
        }

        public static async ValueTask<ATResult<OkResult>> EnterPinAsync(
            this ICommunicator comm,
            ResponseFormat responseFormat,
            Pin pin,
            CancellationToken cancellationToken = default)
        {
            await comm.Write($"AT+CPIN={pin}\r\n", cancellationToken);
            var message = await comm.ReadLineAsync(cancellationToken);
            if (OkParser.TryParse(message, responseFormat, out ATResult<OkResult> okResult))
                return okResult;
            else if (ErrorParser.TryParse(message, responseFormat, out ATResult<ErrorResult> errorResult))
                return ATResult.Error<OkResult>(errorResult.ErrorMessage);
            return ATResult.Error<OkResult>(Constants.PARSING_FAILED);
        }
    }
}

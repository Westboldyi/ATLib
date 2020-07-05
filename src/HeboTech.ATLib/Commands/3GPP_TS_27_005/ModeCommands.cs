﻿using HeboTech.ATLib.Communication;
using HeboTech.ATLib.Parsers;
using HeboTech.ATLib.Results;
using HeboTech.ATLib.States;
using System.Threading;
using System.Threading.Tasks;

namespace HeboTech.ATLib.Commands._3GPP_TS_27_005
{
    public static class ModeCommands
    {
        public static async ValueTask<ATResult<OkResult>> ReadModeAsync(
            this ICommunicator comm,
            ResponseFormat responseFormat,
            CancellationToken cancellationToken = default)
        {
            await comm.Write($"AT+CMGF?\r", cancellationToken);
            var message = await comm.ReadLineAsync(cancellationToken);
            if (OkParser.TryParse(message, responseFormat, out ATResult<OkResult> okResult))
                return okResult;
            else if (ErrorParser.TryParse(message, responseFormat, out ATResult<ErrorResult> errorResult))
                return ATResult.Error<OkResult>(errorResult.ErrorMessage);
            return ATResult.Error<OkResult>(Constants.PARSING_FAILED);
        }

        public static async ValueTask<ATResult<OkResult>> SetModeAsync(
            this ICommunicator comm,
            ResponseFormat responseFormat,
            Mode mode,
            CancellationToken cancellationToken = default)
        {
            await comm.Write($"AT+CMGF={(int)mode}\r", cancellationToken);
            var message = await comm.ReadLineAsync(cancellationToken);
            if (OkParser.TryParse(message, responseFormat, out ATResult<OkResult> okResult))
                return okResult;
            else if (ErrorParser.TryParse(message, responseFormat, out ATResult<ErrorResult> errorResult))
                return ATResult.Error<OkResult>(errorResult.ErrorMessage);
            return ATResult.Error<OkResult>(Constants.PARSING_FAILED);
        }
    }
}

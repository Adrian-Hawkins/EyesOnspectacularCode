namespace EOSC.Common.Requests;

public record DatetimeRequest(
    string dateTimeString, 
    string originalFormat,
    string desiredFormat
);
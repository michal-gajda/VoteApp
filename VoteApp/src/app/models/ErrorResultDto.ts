import { VoteAppErrorCodes } from ".";


export interface ErrorResultDto {
    messages: string[];
    errorCode: VoteAppErrorCodes;
    errorCodeName: string;
    errorLogId: string;
    techDetails?: TechDetails;
}

export interface TechDetails {
    methodName: string;
    exception: string;
    innerException?: string;
}
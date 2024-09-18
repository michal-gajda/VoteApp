export enum VoteAppErrorCodes {
    NotAuthorized = 0,
    PermissionDenied = 1,
    AuthenticationFailed = 2,
    NotExists = 10,
    AlreadyExists = 11,
    DataNotValid = 12,
    ServerError = 100,
    LockTimout = 101,
    DbTimeoutError = 102,
    Other = 1000,
    NotCategorized = 1001
}
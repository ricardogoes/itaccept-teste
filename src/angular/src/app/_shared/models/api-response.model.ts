export class ApiResponse {
    State: number;
    Message: string;
    Data: Object;

    constructor(State: number, Message: string, Data: object) {
        this.State = State;
        this.Message = Message;
        this.Data = Data;
    }
}

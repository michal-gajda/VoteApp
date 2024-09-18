import { Subscription } from "rxjs";

export class Subscriptions extends Subscription {

    set addSubscription(subscription: Subscription) {
        this.add(subscription);
    }

}
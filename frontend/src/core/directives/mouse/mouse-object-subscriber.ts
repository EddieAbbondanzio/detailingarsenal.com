import { MouseActionFunction } from '@/core/directives/mouse/mouse-action-function';
import { MouseButton } from '@/core/directives/mouse/mouse-button';
import { MouseAction } from '@/core/directives/mouse/mouse-action';

export class MouseObjectSubscriber {
    constructor(public action: MouseAction, public callback: MouseActionFunction, public button: MouseButton) {}

    isMatch(action: MouseAction, button: MouseButton) {
        return this.action == action && (this.button == button || this.button == 'either');
    }
}

export function triggerMouseEvent(node: HTMLElement, eventType: 'mouseover' | 'mousedown' | 'click' | 'mouseup') {
    var clickEvent = document.createEvent('MouseEvents');
    clickEvent.initEvent(eventType, true, true);
    node.dispatchEvent(clickEvent);
}

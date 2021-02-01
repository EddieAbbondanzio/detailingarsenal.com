import { DirectiveFunction } from 'vue';

/**
 * Directive that will attempt to focus an input element.
 */
export const focusDirective = {
    // directive definition
    inserted: function(el: HTMLElement, binding: any): void {
        recursiveCheck(el);

        /*
         * Recursively check for an input element, and if we find it, focus it.
         */
        function recursiveCheck(el: HTMLElement) {
            if (el.tagName == 'INPUT') {
                el.focus();
            } else {
                for (let child of el.children) {
                    recursiveCheck(child as HTMLElement);
                }
            }
        }
    }
};

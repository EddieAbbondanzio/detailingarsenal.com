import Vue from 'vue';

export const disableAll = {
    // When all the children of the parent component have been updated
    componentUpdated: function(el: any, binding: any) {
        if (!binding.value) {
            return;
        }

        const tags = ['input', 'button', 'textarea', 'select'];
        tags.forEach(tagName => {
            const nodes = el.getElementsByTagName(tagName);
            for (let i = 0; i < nodes.length; i++) {
                (nodes[i] as any).disabled = true;
                (nodes[i] as any).tabIndex = -1;
            }
        });
    }
};

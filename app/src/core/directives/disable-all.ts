import Vue from 'vue';

export const disableAll = {
    // When all the children of the parent component have been updated
    componentUpdated: function(el: any, binding: any) {
        const tags = ['input', 'button', 'textarea', 'select'];
        tags.forEach(tagName => {
            const nodes = el.getElementsByTagName(tagName);
            for (let i = 0; i < nodes.length; i++) {
                (nodes[i] as any).disabled = binding.value;
            }
        });
    }
};

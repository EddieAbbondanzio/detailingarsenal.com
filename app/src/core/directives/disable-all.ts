import Vue from 'vue';

export const disableAll = {
    // When all the children of the parent component have been updated
    componentUpdated: function(el: any, binding: any) {
        const tags = ['input', 'button', 'textarea', 'select'];
        tags.forEach(tagName => {
            const nodes = el.getElementsByTagName(tagName);
            for (let i = 0; i < nodes.length; i++) {
                // cache original disabled state
                if (nodes[i].wasDisabled == null) {
                    nodes[i].wasDisabled = nodes[i].disabled;
                }

                // check for a previously disabled component before re-enabling it.
                nodes[i].disabled = nodes[i].wasDisabled ? true : binding.value;
            }
        });
    }
};

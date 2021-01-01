import { Component, Vue, Prop } from 'vue-property-decorator';
import { RawLocation } from 'vue-router';

@Component({
    name: 'button-mixin'
})
export default class InputViewMixin extends Vue {
    get id() {
        return this.$route.params.id;
    }

    get mode() {
        return this.$route.params.id == null ? 'create' : 'update';
    }

    get verb() {
        switch (this.mode) {
            case 'create':
                return 'Create';
            case 'update':
                return 'Update';
        }
    }
}

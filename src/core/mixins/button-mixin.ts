import { Component, Vue, Prop } from 'vue-property-decorator';
import { RawLocation } from 'vue-router';

@Component({
    name: 'button-mixin'
})
export default class ButtonMixin extends Vue {
    @Prop()
    to!: RawLocation;

    @Prop({ default: '' })
    icon!: string;

    @Prop({ default: '' })
    text!: string;

    @Prop({ default: false })
    loading!: boolean;

    @Prop({ default: false })
    outlined!: boolean;

    @Prop({ default: false })
    disabled!: boolean;

    get tag() {
        return this.to != null ? 'router-link' : 'button';
    }
}

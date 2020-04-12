import { ModalProgrammatic } from 'buefy';
import ConfirmDeleteModal from './confirm-delete-modal.vue';

export function confirmDelete(entityType: string, identifier: string): Promise<boolean> {
    return new Promise<boolean>((res, rej) => {
        var obj = ModalProgrammatic.open({
            props: { entityType, identifier },
            component: ConfirmDeleteModal,
            events: {
                confirm: () => {
                    obj.close();
                    res(true);
                },
                cancel: () => {
                    obj.close();
                    res(false);
                }
            }
        });
    });
}

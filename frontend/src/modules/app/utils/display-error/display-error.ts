import { ModalProgrammatic } from 'buefy';
import ErrorModal from './error-modal.vue';

/**
 * Display en error message as a big scary pop up.
 * @param error The error or message to display.
 */
export function displayError(error: Error | string) {
    const errorMessage = error instanceof Error ? error.message : error;

    var obj = ModalProgrammatic.open({
        props: { message: errorMessage },
        component: ErrorModal,
        events: {
            close: () => {
                obj.close();
            }
        }
    });
}

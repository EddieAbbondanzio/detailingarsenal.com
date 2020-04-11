import { ToastProgrammatic as Toast } from 'buefy';

/**
 * Display a toast on the screen.
 * @param message The message to display.
 * @param opts Display options
 */
export function toast(message: string, opts: { duration?: number; type?: string } = { type: 'is-primary' }) {
    Toast.open({ message, type: opts.type, duration: opts.duration, position: 'is-bottom' });
}

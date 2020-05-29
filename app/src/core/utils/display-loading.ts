import appStore from '@/core/store/app-store';

/**
 * Indicate a function is loading by triggering the on screen loading icon.
 * Loading goes from true to false.
 */
export function displayLoading(
    target: any,
    propertyKey: string,
    descriptor: TypedPropertyDescriptor<(...params: any[]) => Promise<any>>
) {
    const oldFunction = descriptor.value;

    descriptor.value = async function() {
        appStore.LOADING();

        if (oldFunction != null) {
            //@ts-ignore
            await oldFunction.apply(this, arguments);
        }

        appStore.LOADED();
    };
}

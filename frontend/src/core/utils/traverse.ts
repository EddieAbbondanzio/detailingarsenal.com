/**
 * Recursively iterate an object.
 * @param obj The object to iterate
 * @param fn The map function that is called for each property iterated.
 */
export function traverse(obj: any, fn: (obj: any, prop: string, value: any) => void) {
    for (const key in obj) {
        //@ts-ignore
        fn.apply(this, [obj, key, obj[key]]);
        if (obj[key] != null && typeof obj[key] == 'object') {
            traverse(obj[key], fn);
        }
    }
}

export function uppercaseFirst(word: string | string[] | null): any {
    if (word == null) {
        return '';
    } else if (typeof word == 'string') {
        return word.charAt(0).toUpperCase() + word.slice(1);
    } else {
        return word.map(w => uppercaseFirst(w));
    }
}

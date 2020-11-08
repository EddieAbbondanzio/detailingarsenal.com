export function uppercase_first(word: string) {
    if (word == null) {
        return '';
    }

    return word.charAt(0).toUpperCase() + word.slice(1);
}

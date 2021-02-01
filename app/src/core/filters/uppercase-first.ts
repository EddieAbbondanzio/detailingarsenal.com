export function uppercaseFirst(word: string) {
    if (word == null) {
        return null;
    }

    return word.charAt(0).toUpperCase() + word.slice(1);
}

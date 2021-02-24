export const ArrayUtils = {
    unique(array: any[], matchOn: (v: any) => any) {
        return array.filter(onlyUnique);

        function onlyUnique(value: any, index: number, self: any[]) {
            return self.findIndex((v: any, i: number, a: any[]) => matchOn(v)) == index;
        }
    }
};

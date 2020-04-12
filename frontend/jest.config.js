module.exports = {
    moduleFileExtensions: [
        'js',
        'ts',
        'json',
        // tell Jest to handle `*.vue` files
        'vue'
    ],
    modulePaths: ['<rootDir>'],
    transform: {
        // process `*.vue` files with `vue-jest`
        '.*\\.(vue)$': 'vue-jest',
        '^.+\\.tsx?$': 'ts-jest'
    },
    testURL: 'http://localhost/',
    testRegex: '(/tests/.*|(\\.|/)(test|spec))\\.(jsx?|tsx?)$'
};

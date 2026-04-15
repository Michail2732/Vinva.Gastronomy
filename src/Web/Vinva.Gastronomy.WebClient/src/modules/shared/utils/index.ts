function getFirstNWords(str: string, wordsCnt: number) : string
{
    if (!str || str.trim().length === 0) {
        return "";
    }
    const words = str.trim().split(/\s+/);
    const firstNWords = words.slice(0, wordsCnt).join(' ');
    return firstNWords;
}

function skipFirstNWords(str: string, wordsCnt: number) : string
{
     if (!str || str.trim().length === 0) {
        return "";
    }
    const words = str.trim().split(/\s+/);
    const firstNWords = words.slice(wordsCnt).join(' ');
    return firstNWords;
}

export default
{
    getFirstNWords,
    skipFirstNWords
};
const getAndUpdate = async (
    url,
    setStateCallback,
    setLoaded,
    errorCallback) => {
    setLoaded && setLoaded(false);
    try {
        let resp = await fetch(url);
        handleErrors(resp);
        resp = await resp.json();
        setStateCallback(resp);
    } catch (errorMessage) {
        if (errorCallback) {
            errorCallback(errorMessage);
        } else {
            console.log("Error during getAndUpdate")
            // notification.error({ message: errorMessage.message });
        }
    } finally {
        setLoaded && setLoaded(true);
    }
};


const handleErrors = response => {
    if (!response.ok) {
        throw Error(response.statusText);
    }
    return response;
};

export default getAndUpdate;

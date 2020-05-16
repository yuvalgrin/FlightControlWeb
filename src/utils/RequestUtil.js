export const getAndUpdate = async (
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
            errorCallback(errorMessage.toString());
        } else {
            console.log("Error during getAndUpdate")
        }
    } finally {
        setLoaded && setLoaded(true);
    }
};

export const deleteReq = async (url, errorCallback) =>  {
    try {

        let resp = await fetch(url, {
            method: 'delete'
        })
    } catch (errorMessage) {
        if (errorCallback) {
            errorCallback(errorMessage);
        } else {
            console.log("Error during getAndUpdate")
        }
    }
}

export const postReq = async (url, data, errorCallback) =>  {
    try {

        let resp = await fetch(url, {
        method: 'post',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
        })
    } catch (errorMessage) {
        if (errorCallback) {
            errorCallback(errorMessage);
        } else {
            console.log("Error during getAndUpdate")
        }
    }
}


const handleErrors = response => {
    if (!response.ok) {
        throw Error(response.statusText);
    }
    return response;
};
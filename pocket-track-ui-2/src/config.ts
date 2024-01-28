let POCKET_TRACK_API_URL = 'http://localhost:5116';

if (process.env.REACT_APP_ENV === 'test') {
    POCKET_TRACK_API_URL = 'http://localhost:5116';
} else if (process.env.REACT_APP_ENV === 'development') {
    POCKET_TRACK_API_URL = 'http://localhost:5116';
}

const config = {
    POCKET_TRACK_API_URL,
};

export default config;
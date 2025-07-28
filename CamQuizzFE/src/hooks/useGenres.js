import { useState, useEffect } from "react";
import { getGenres } from "../features/quiz/genreApi";
export const useGenres = (home=true) => {
    const [genres, setGenres] = useState([])
    const [errGenres, setErrGenres] = useState(null)
    useEffect(() => {
        const fetchGenres = async () => {
            try {
                setErrGenres(null);
                const result = await getGenres();
                if(home)
                    setGenres([{ id: 'All', name: 'All' }, ...result]);
                else
                    setGenres([ ...result]);

            } catch (err) {
                setErrGenres(err);
            }
        };

        fetchGenres();
    }, []);

    return { genres, errGenres }
}
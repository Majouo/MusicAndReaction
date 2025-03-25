// App.js
import React, { useState, useRef, useEffect } from 'react';
import './App.css';
import Waveform from './Waveform'; // Import nowego komponentu

// Komponent mierz¹cy czas reakcji
const ReactionTimer = ({ stopTime, wavesurfer }) => {
    const [reactionTime, setReactionTime] = useState(null);
    const [musicPlaying, setMusicPlaying] = useState(false);
    const [canReact, setCanReact] = useState(false);
    const startTimeRef = useRef(null);

    const startSession = () => {
        if (wavesurfer.current) {
            wavesurfer.current.play();
        }
        setMusicPlaying(true);
        setReactionTime(null);
        setCanReact(false);

        // Zatrzymanie muzyki po czasie podanym przez backend (stopTime)
        setTimeout(() => {
            if (wavesurfer.current) {
                wavesurfer.current.pause();
            }
            setMusicPlaying(false);
            startTimeRef.current = Date.now();
            setCanReact(true);
        }, stopTime);
    };

    const handleUserReaction = async () => {
        if (canReact) {
            const reaction = Date.now() - startTimeRef.current;
            setReactionTime(reaction);
            setCanReact(false);

            // Wysy³amy wynik do backendu
            try {
                await fetch('api/reaction', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ reactionTime: reaction }),
                });
            } catch (error) {
                console.error('B³¹d wysy³ania wyniku:', error);
            }
        }
    };

    return (
        <div style={{ paddingBottom: '100px' }}>
            <h2>Test reakcji</h2>
            <button onClick={canReact ? handleUserReaction : startSession} disabled={musicPlaying}>
                {musicPlaying ? "Muzyka gra..." : canReact ? "Kliknij!" : "Rozpocznij test"}
            </button>

            {reactionTime !== null && (
                <p>Twój czas reakcji: {reactionTime} ms</p>
            )}
        </div>
    );
};

// G³ówny komponent aplikacji
const App = () => {
    const [trackUrl, setTrackUrl] = useState(null);
    const [stopTime, setStopTime] = useState(null);
    const wavesurferRef = useRef(null);

    const fetchMusicSession = async () => {
        try {
            const response = await fetch('api/MusicSession');
            if (!response.ok) {
                throw new Error(`B³¹d API: ${response.status} ${response.statusText}`);
            }
            const data = await response.json();
            setTrackUrl(data.trackUrl);
            setStopTime(data.stopTime);
        } catch (error) {
            console.error('B³¹d pobierania sesji muzycznej:', error);
        }
    };

    useEffect(() => {
        fetchMusicSession();
    }, []);

    return (
        <div>
            <ReactionTimer stopTime={stopTime} wavesurfer={wavesurferRef} />
            {trackUrl && <Waveform audioUrl={trackUrl} wavesurferRef={wavesurferRef} />}
        </div>
    );
};

export default App;

import React, { useState, useRef, useEffect } from 'react';
import './App.css';
import Waveform from './Waveform';

// Komponent mierz�cy czas reakcji w dw�ch trybach (afterStart i afterStop)
const ReactionTimer = ({ stopDelay, wavesurfer }) => {
    const [mode, setMode] = useState('afterStop');
    const [reactionTime, setReactionTime] = useState(null);
    const [musicPlaying, setMusicPlaying] = useState(false);
    const [canReact, setCanReact] = useState(false);
    const [elapsedTime, setElapsedTime] = useState(0);
    const startTimeRef = useRef(null);
    const playTimeoutRef = useRef(null);
    const pauseTimeoutRef = useRef(null);
    const intervalRef = useRef(null);

    const clearAll = () => {
        clearTimeout(playTimeoutRef.current);
        clearTimeout(pauseTimeoutRef.current);
        clearInterval(intervalRef.current);
    };

    const startSession = () => {
        if (!wavesurfer.current) return;

        // Reset stan�w i timeout�w
        clearAll();
        setReactionTime(null);
        setElapsedTime(0);
        setCanReact(false);

        // Zablokuj przycisk start
        setMusicPlaying(true);

        if (mode === 'afterStop') {
            // Tryb: pomiar po zako�czeniu
            wavesurfer.current.seekTo(0);
            wavesurfer.current.play();

            // Pauza i start pomiaru po stopDelay
            pauseTimeoutRef.current = setTimeout(() => {
                wavesurfer.current.pause();
                setMusicPlaying(false);
                startTimeRef.current = Date.now();
                setCanReact(true);
                intervalRef.current = setInterval(() => {
                    setElapsedTime(Date.now() - startTimeRef.current);
                }, 16);
            }, stopDelay);
        } else {
            // Tryb: pomiar po rozpocz�ciu
            // Odtwarzanie op�nione o stopDelay
            playTimeoutRef.current = setTimeout(() => {
                wavesurfer.current.seekTo(0);
                wavesurfer.current.play();
                setMusicPlaying(true);

                // Start pomiaru
                startTimeRef.current = Date.now();
                setCanReact(true);
                intervalRef.current = setInterval(() => {
                    setElapsedTime(Date.now() - startTimeRef.current);
                }, 16);
            }, stopDelay);
        }
    };

    const handleUserReaction = async () => {
        if (!canReact) return;
        const reaction = Date.now() - startTimeRef.current;
        setReactionTime(reaction);
        setCanReact(false);

        // Cleanup
        clearAll();
        if (wavesurfer.current) {
            wavesurfer.current.pause();
            setMusicPlaying(false);
        }

        try {
            await fetch('api/reaction', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ reactionTime: reaction, mode }),
            });
        } catch (error) {
            console.error('B��d wysy�ania wyniku:', error);
        }
    };

    useEffect(() => () => clearAll(), []);

    return (
        <div style={{ paddingBottom: '100px' }}>
            <h2>Test reakcji</h2>
            <div style={{ marginBottom: '10px' }}>
                <label>
                    Tryb:{' '}
                    <select
                        value={mode}
                        onChange={e => setMode(e.target.value)}
                        disabled={musicPlaying || canReact}
                    >
                        <option value="afterStop">Po zako�czeniu muzyki</option>
                        <option value="afterStart">Po rozpocz�ciu muzyki</option>
                    </select>
                </label>
            </div>
            <button onClick={startSession} disabled={musicPlaying || canReact}>
                Rozpocznij test
            </button>
            <button
                onClick={handleUserReaction}
                disabled={!canReact}
                style={{ marginLeft: '10px' }}
            >
                Kliknij!
            </button>

            {canReact && (
                <p>
                    Czas od {mode === 'afterStop' ? 'zako�czenia' : 'rozpocz�cia'} muzyki: {elapsedTime} ms
                </p>
            )}
            {reactionTime !== null && <p>Tw�j czas reakcji: {reactionTime} ms</p>}
        </div>
    );
};

// G��wny komponent aplikacji
const App = () => {
    const [trackUrl, setTrackUrl] = useState(null);
    const [stopDelay, setStopDelay] = useState(null);
    const wavesurferRef = useRef(null);

    const fetchMusicSession = async () => {
        try {
            const response = await fetch('api/MusicSession');
            if (!response.ok) {
                throw new Error(`B��d API: ${response.status} ${response.statusText}`);
            }
            const data = await response.json();
            setTrackUrl(data.trackUrl);
            setStopDelay(data.stopTime);
        } catch (error) {
            console.error('B��d pobierania sesji muzycznej:', error);
        }
    };

    useEffect(() => {
        fetchMusicSession();
    }, []);

    if (trackUrl === null || stopDelay === null) {
        return <p>�adowanie sesji muzycznej...</p>;
    }

    return (
        <div>
            <ReactionTimer stopDelay={stopDelay} wavesurfer={wavesurferRef} />
            <Waveform audioUrl={trackUrl} wavesurferRef={wavesurferRef} />
        </div>
    );
};

export default App;
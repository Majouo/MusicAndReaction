import React, { useState, useRef, useEffect } from 'react';
import { useSpring, animated } from '@react-spring/web';
import Waveform from './Waveform';
import './App.css';

// Komponent mierz¹cy czas reakcji
const ReactionTimer = ({ stopDelay, wavesurfer, trackId, onTriggerFlash }) => {
    const [mode, setMode] = useState('afterStop');
    const [reactionTime, setReactionTime] = useState(null);
    const [musicPlaying, setMusicPlaying] = useState(false);
    const [canReact, setCanReact] = useState(false);
    const [elapsedTime, setElapsedTime] = useState(0);

    const startTimeRef = useRef(null);
    const playTimeoutRef = useRef(null);
    const pauseTimeoutRef = useRef(null);

    // Animacja rosn¹cej wartoœci elapsedTime
    const { number } = useSpring({
        from: { number: 0 },
        number: elapsedTime,
        config: { tension: 170, friction: 26 }
    });

    // Start pomiaru i interwa³u
    useEffect(() => {
        let interval;
        if (canReact) {
            onTriggerFlash();
            interval = setInterval(() => {
                setElapsedTime(Date.now() - startTimeRef.current);
            }, 16);
        }
        return () => clearInterval(interval);
    }, [canReact, onTriggerFlash]);

    const clearAll = () => {
        clearTimeout(playTimeoutRef.current);
        clearTimeout(pauseTimeoutRef.current);
    };

    const startSession = () => {
        if (!wavesurfer.current) return;
        clearAll();
        setReactionTime(null);
        setElapsedTime(0);
        setCanReact(false);
        setMusicPlaying(true);

        const startNow = () => {
            wavesurfer.current.seekTo(0);
            wavesurfer.current.play();
            setMusicPlaying(true);
            startTimeRef.current = Date.now();
            setCanReact(true);
        };

        if (mode === 'afterStop') {
            wavesurfer.current.seekTo(0);
            wavesurfer.current.play();
            pauseTimeoutRef.current = setTimeout(() => {
                wavesurfer.current.pause();
                setMusicPlaying(false);
                startNow();
            }, stopDelay);
        } else {
            playTimeoutRef.current = setTimeout(startNow, stopDelay);
        }
    };

    const handleUserReaction = async () => {
        if (!canReact) return;
        const reaction = Date.now() - startTimeRef.current;
        setReactionTime(reaction);
        setCanReact(false);
        clearAll();
        if (wavesurfer.current) {
            wavesurfer.current.pause();
            setMusicPlaying(false);
        }
        try {
            await fetch('api/reaction', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ reactionTime: reaction, mode, trackId }),
            });
        } catch (error) {
            console.error('B³¹d wysy³ania wyniku:', error);
        }
    };

    // obs³uga spacji do reakcji
    useEffect(() => {
        const handleKeyDown = (e) => {
            if (e.code === 'Space' && canReact) {
                e.preventDefault();
                handleUserReaction();
            }
        };
        window.addEventListener('keydown', handleKeyDown);
        return () => window.removeEventListener('keydown', handleKeyDown);
    }, [canReact]);

    return (
        <div style={{ padding: '20px' }}>
            <h2>Test reakcji</h2>
            <div style={{ marginBottom: '10px' }}>
                <label>
                    Tryb:{' '}
                    <select
                        value={mode}
                        onChange={e => setMode(e.target.value)}
                        disabled={musicPlaying || canReact}
                    >
                        <option value="afterStop">Po zakoñczeniu muzyki</option>
                        <option value="afterStart">Po rozpoczêciu muzyki</option>
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
                <p style={{ marginTop: '20px' }}>
                    Czas od {mode === 'afterStop' ? 'zakoñczenia' : 'rozpoczêcia'} muzyki:{' '}
                    <animated.span>{number.to(n => Math.floor(n))}</animated.span> ms
                </p>
            )}
            {reactionTime !== null && <p>Twój czas reakcji: {reactionTime} ms</p>}
        </div>
    );
};

// G³ówny komponent aplikacji
const App = () => {
    const [trackUrl, setTrackUrl] = useState(null);
    const [stopDelay, setStopDelay] = useState(null);
    const [trackId, setTrackId] = useState(null);
    const wavesurferRef = useRef(null);
    const [flash, setFlash] = useState(false);

    // Pe³noekranowy overlay przyciemnienia
    const overlayStyle = useSpring({
        from: { opacity: 0 },
        to: { opacity: flash ? 0.5 : 0 },
        config: { tension: 210, friction: 20 }
    });

    useEffect(() => {
        if (flash) {
            const t = setTimeout(() => setFlash(false), 200);
            return () => clearTimeout(t);
        }
    }, [flash]);

    const fetchMusicSession = async () => {
        try {
            const response = await fetch('api/MusicSession');
            if (!response.ok) throw new Error(`B³¹d API: ${response.status}`);
            const data = await response.json();
            setTrackUrl(data.trackUrl);
            setStopDelay(data.stopTime);
            setTrackId(data.trackId);
        } catch (error) {
            console.error('B³¹d pobierania sesji muzycznej:', error);
        }
    };

    useEffect(() => { fetchMusicSession(); }, []);
    if (!trackUrl || stopDelay == null || !trackId) return <p>£adowanie sesji muzycznej...</p>;

    return (
        <>
            <animated.div
                style={{
                    ...overlayStyle,
                    position: 'fixed',
                    top: 0,
                    left: 0,
                    width: '100%',
                    height: '100%',
                    backgroundColor: '#76bedb',
                    pointerEvents: 'none',
                    zIndex: 9999
                }}
            />
            <ReactionTimer
                stopDelay={stopDelay}
                wavesurfer={wavesurferRef}
                trackId={trackId}
                onTriggerFlash={() => setFlash(true)}
            />
            <Waveform audioUrl={trackUrl} wavesurferRef={wavesurferRef} />
        </>
    );
};

export default App;
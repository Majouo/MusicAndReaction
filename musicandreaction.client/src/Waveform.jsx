// Waveform.js
import React, { useRef, useEffect } from 'react';
import WaveSurfer from 'wavesurfer.js';

const Waveform = ({ audioUrl, wavesurferRef }) => {
    const waveformContainerRef = useRef(null);

    useEffect(() => {
        // Inicjalizacja WaveSurfer przy montowaniu komponentu
        wavesurferRef.current = WaveSurfer.create({
            container: waveformContainerRef.current,
            waveColor: '#ddd',
            progressColor: '#00a9ff',
            height: 160,
            responsive: true,
            barWidth: 2,
            cursorWidth: 1,
            cursorColor: '#ff5500',
            interact: false, 
        });

        if (audioUrl) {
            wavesurferRef.current.load(audioUrl);
        }

        return () => {
            if (wavesurferRef.current) {
                wavesurferRef.current.destroy();
            }
        };
    }, [audioUrl, wavesurferRef]);

    return (
        <div
            ref={waveformContainerRef}
            style={{ width: '100%', position: 'fixed', bottom: 0, left: 0 }}
        ></div>
    );
};

export default Waveform;

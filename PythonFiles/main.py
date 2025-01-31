import cv2
import pretty_midi
from scipy.io.wavfile import write
from pydub import AudioSegment
import numpy as np
from music21 import stream, note, chord, midi


class MusicRecognizer:
    def __init__(self):
        """
        Класс для распознавания нот на изображении и их преобразования в аудио.
        """
        pass

    def midi_to_mp3(self, midi_path, mp3_path):
        # Создаём объект MIDI
        midi_data = pretty_midi.PrettyMIDI(midi_path)

        # Синтезируем WAV из MIDI (используется простейший синтезатор)
        audio_data = midi_data.synthesize()

        # Сохраняем WAV файл
        wav_path = "temp_output.wav"
        write(wav_path, 44100, audio_data)

        # Преобразуем WAV в MP3 через pydub
       # sound = AudioSegment.from_file(wav_path, format="wav")
       # sound.export(mp3_path, format="mp3")
        print('OKOKOKOKOKOK')
       # print(f"MP3 файл успешно сохранён: {mp3_path}")

    def preprocess_image(self, image_path):
        """
        Обработка изображения: преобразование в черно-белое и бинаризация.

        :param image_path: Путь к изображению с нотами.
        :return: Бинаризированное изображение.
        """
        # Загрузка изображения в градациях серого
        img = cv2.imread(image_path, cv2.IMREAD_GRAYSCALE)

        # Применяем бинаризацию для выделения объектов
        _, binary = cv2.threshold(img, 128, 255, cv2.THRESH_BINARY_INV)

        return binary

    def detect_notes(self, binary_image):
        """
        Детектирование нот на изображении. Упрощённо распознаются позиции объектов.

        :param binary_image: Бинаризированное изображение.
        :return: Список нот (упрощённо — фиксированные высоты).
        """
        # Поиск контуров (каждый контур — потенциальная нота)
        contours, _ = cv2.findContours(
            binary_image, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE
        )

        notes = []
        for contour in contours:
            # Получаем прямоугольник вокруг контура
            x, y, w, h = cv2.boundingRect(contour)

            # Упрощённо определяем высоту ноты по позиции
            if h > 10 and w > 10:  # Игнорируем шум
                # Пример: если нота в верхней части изображения — это "до"
                pitch = self.get_pitch_from_position(y)
                notes.append(pitch)

        return notes

    def get_pitch_from_position(self, y):
        """
        Преобразует координаты Y в высоту ноты.
        (Упрощённый пример для нот до, ре, ми, фа, соль, ля, си).

        :param y: Координата Y.
        :return: Высота ноты (например, 'C4').
        """
        pitches = ["C4", "D4", "E4", "F4", "G4", "A4", "B4"]
        index = (y // 20) % len(pitches)  # Условный расчёт
        return pitches[index]

    def convert_to_audio(self, notes, output_path):
        """
        Преобразует список нот в MIDI файл.

        :param notes: Список нот (например, ['C4', 'E4', 'G4']).
        :param output_path: Путь для сохранения MIDI файла.
        """
        # Создаём музыкальный поток
        midi_stream = stream.Stream()

        for pitch in notes:
            # Добавляем каждую ноту в поток
            n = note.Note(pitch)
            n.quarterLength = 1  # Продолжительность ноты
            midi_stream.append(n)

        # Сохраняем поток в MIDI файл
        midi_file = midi.translate.music21ObjectToMidiFile(midi_stream)
        midi_file.open(output_path, "wb")
        midi_file.write()
        midi_file.close()
        print(f"MIDI файл успешно сохранён: {output_path}")



def AnalysisStart(image_path, midi_path):
    recognizer = MusicRecognizer()
# Шаг 1: Обработка изображения
    binary_image = recognizer.preprocess_image(image_path)
# Шаг 2: Распознавание нот
    notes = recognizer.detect_notes(binary_image)
    print(f"Распознанные ноты: {notes}")
# Шаг 3: Преобразование в MIDI
    recognizer.convert_to_audio(notes, midi_path)
    return "OK"


import sys
import cv2
import os
from music21 import stream, note, midi

class MusicRecognizer:
    def preprocess_image(self, image_path):
        img = cv2.imread(image_path, cv2.IMREAD_GRAYSCALE)
        if img is None:
            print("FILE IS NONE")
            return None
        _, binary = cv2.threshold(img, 128, 255, cv2.THRESH_BINARY_INV)
        #print("END preprocess_image")
        return binary

    def detect_notes(self, binary_image):
        if binary_image is None:
            return []
        contours, _ = cv2.findContours(binary_image, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
        notes = []
        for contour in contours:
            x, y, w, h = cv2.boundingRect(contour)
            if h > 10 and w > 10:
                pitch = self.get_pitch_from_position(y)
                notes.append(pitch)
        #print(f"END detect_notes: {notes}")
        if notes == None:
            print("Note not found")
        return notes

    def get_pitch_from_position(self, y):
        pitches = ["C4", "D4", "E4", "F4", "G4", "A4", "B4"]
        index = min(max(y // 20, 0), len(pitches) - 1)
        return pitches[index]

    def convert_to_audio(self, notes, output_path):
        if len(notes) == 0:
            print("ERROR")
            return

        try:
            midi_stream = stream.Stream()

            for pitch in notes:
                n = note.Note(pitch)
                n.quarterLength = 1
                midi_stream.append(n)

            # Создаём MIDI-файл
            midi_file = midi.translate.music21ObjectToMidiFile(midi_stream)

            # Проверяем, существует ли папка для сохранения файла
            output_dir = os.path.dirname(output_path)
            if output_dir and not os.path.exists(output_dir):
                os.makedirs(output_dir)

            # Записываем MIDI-файл
            midi_file.open(output_path, "wb")  # Открываем файл для записи
            midi_file.write()  # Записываем данные
            midi_file.close()  # Закрываем файл
            #print(f"✅ MIDI файл успешно сохранён: {output_path}")

        except Exception as e:
            print(f"❌ Ошибка при создании MIDI: {e}")

def get(image_paths, output_midi):
    notes_end = []
    recognizer = MusicRecognizer()
    for path in image_paths:
        binary_image = recognizer.preprocess_image(path)
        if binary_image is not None:
            notes_end.extend(recognizer.detect_notes(binary_image))
    recognizer.convert_to_audio(notes_end, output_midi)

if __name__ == "__main__":
    #sys.stdout = open(os.devnull, 'w')  # Отключаем вывод в консоль
    #sys.stderr = open(os.devnull, 'w')  # Отключаем вывод ошибок

    # Принимаем аргументы в виде списка
    image_paths = sys.argv[1:-1]  # Все аргументы, кроме последнего, — это пути к изображениям
    output_midi = sys.argv[-1]  # Последний аргумент — путь для сохранения MIDI
    #print("START ANALYSIS")
    #print(f"arg 1: {image_paths}")
    #print(f"arg 2: {output_midi}")
    get(image_paths, output_midi)



import sys
import rembg
import io

def remove_background(input_path, output_path):
    with open(input_path, "rb") as inp_file:
        input_data = inp_file.read()
        output_data = rembg.remove(input_data)
    
    with open(output_path, "wb") as out_file:
        out_file.write(output_data)

if __name__ == "__main__":
    if len(sys.argv) != 3:
        print("Usage: python script.py <input_image> <output_image>")
        sys.exit(1)

    input_image = sys.argv[1]
    output_image = sys.argv[2]

    remove_background(input_image, output_image)

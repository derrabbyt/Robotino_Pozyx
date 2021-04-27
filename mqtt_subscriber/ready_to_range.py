import numpy as np
import matplotlib.pyplot as plt

img = plt.imread("background.jpeg")
ext = [0.0, 7468.0, 0.00, 9650.0]
plt.imshow(img, zorder=0, extent=ext)
aspect=img.shape[0]/float(img.shape[1])*((ext[1]-ext[0])/(ext[3]-ext[2]))
plt.gca().set_aspect(aspect)


def animate(i):
    x_vals.append(next(index))
    y_vals.append(random.randint(0, 5))

    plt.cla()
    plt.plot(x_vals, y_vals)

plt.show()
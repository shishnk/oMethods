from pylab import figure, cm

import matplotlib.pyplot as plt
import numpy as np


def f(x1,x2):
    return ((2.0 / (1 + ((x1 - 5) * (x1 - 5)) + ((x2 - 4) * (x2 - 4)))) +
           (1.0 / (1 + ((x1 - 2) * (x1 - 2)) + (x2 * x2))) +
           (7.0 / (1 + ((x1 + 9) * (x1 + 9)) + ((x2 + 6) * (x2 + 6)))) +
           (2.0 / (1 + (x1 * x1) + ((x2 + 3) * (x2 + 3)))) +
           (8.0 / (1 + ((x1 + 3) * (x1 + 3)) + ((x2 - 7) * (x2 - 7)))) +
           (4.0 / (1 + ((x1 + 3) * (x1 + 3)) + ((x2 - 3) * (x2 - 3)))))


x1_min = -10.0
x1_max = 10.0
x2_min = -10.0
x2_max = 10.0

x1, x2 = np.meshgrid(np.arange(x1_min,x1_max, 0.1), np.arange(x2_min,x2_max, 0.1))

y = f(x1,x2)

plt.imshow(y,extent=[x1_min,x1_max,x2_min,x2_max], cmap=cm.jet, origin='lower')

plt.colorbar()


plt.show()
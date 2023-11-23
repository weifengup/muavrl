import matplotlib.pyplot as plt
import pickle
import numpy as np
def draw(file_path):
    # 从 pkl.npy 文件中加载数据
    with open(file_path, 'rb') as file:
        data = np.load(file_path)
        print(data)

    # 假设数据是一个 Numpy 数组，可以直接绘制
    plt.plot(data)
    plt.show()

if __name__ == '__main__':
    file_path='/Users/hanlinfeng/unity/muavrl/Assets/Python/maddpg/model/single_agent_single_object_1122j/returns.pkl.npy'
    draw(file_path)
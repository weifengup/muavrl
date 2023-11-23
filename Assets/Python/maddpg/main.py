from runner import Runner
from common.arguments import get_args
import numpy as np
import random
import torch
import argparse
from peaceful_pie.unity_comms import UnityComms

def getUnityComms(args:argparse.Namespace):
    unityComms=UnityComms(port=args.port)
    return unityComms
def make_env(args):
    from MyUnityEnv import MyUnityEnv
    unityComms=getUnityComms(args)
    env = MyUnityEnv(unityComms)
    args.n_players = env.n  # 包含敌人的所有玩家个数
    args.n_agents = env.n - args.num_adversaries  # 需要操控的玩家个数，虽然敌人也可以控制，但是双方都学习的话需要不同的算法
    args.obs_shape = [env.observation_space[0] for i in range(args.n_agents)]  # 每一维代表该agent的obs维度
    print(args.obs_shape)
    action_shape = []
    for i in range(env.n):
        action_shape.append(3)
    args.action_shape = action_shape[:args.n_agents]  # 每一维代表该agent的act维度
    args.high_action = 1
    args.low_action = -1
    return env, args

if __name__ == '__main__':
    # get the params
    args = get_args()
    env, args = make_env(args)
    runner = Runner(args, env)
    if args.evaluate:
        returns = runner.evaluate()
        print('Average returns is', returns)
    else:
        runner.run()

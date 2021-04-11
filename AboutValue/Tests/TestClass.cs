using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GameUtility;
using UnityEngine.TestTools.Utils;
using System;

public class TestClass
{
    // A Test behaves as an ordinary method
    [Test]
    public void ModifierTestPasses()
    {
        //Debug.Log("ModifierTest:");
        //�����޼�����޸����Ĵ���
        FloatEqualityComparer fq = new FloatEqualityComparer(10e-8f);
        Memoless meml1 = new Memoless(3.2f);
        Memoless meml2 = new Memoless(4.4f);
        Modifier modi1 = new Modifier(meml1, meml2, ModifyOperations.ADD);
        Debug.Log(modi1.Value);
        Assert.IsTrue(fq.Equals(modi1.Value, 7.6f));
        //�޼����ǹ����壬����޸�������ʱ�յ���ʵ�����Ǹ���
        meml1.Value = 4.4f;
        Assert.IsTrue(fq.Equals(modi1.Value, 7.6f));

        //���Եػ����޸����Ĵ���
        Basement bas1 = new Basement(54.1f);
        Basement bas2 = new Basement(77.5f);
        Modifier modi2 = new Modifier(bas1, bas2, ModifyOperations.ADD);
        Assert.IsTrue(fq.Equals(modi2.Value, 131.6f));
        bas1.Value = 77.6f;
        Assert.IsTrue(fq.Equals(modi2.Value, 155.1f));
        bas2.Value = 1.0f;
        Assert.IsTrue(fq.Equals(modi2.Value, 78.6f));

        //�����޸������Դ���
        Modifier modi3 = new Modifier(modi1, modi2, ModifyOperations.ADD);
        Assert.IsTrue(fq.Equals(modi3.Value, 86.2f));
        //Debug.Log(modi3.Value);
        bas2.Value = 2.0f;
        Assert.IsTrue(fq.Equals(modi3.Value, 87.2f));
        //Debug.Log(modi3.Value);

        //�����޸�������ֵ����ָ��
        modi3.SetPartnerValue(new Memoless(3.3f));
        Assert.IsTrue(fq.Equals(modi3.Value, 10.9f));
        bas2.Value = 3.0f;
        Assert.IsTrue(fq.Equals(modi3.Value, 10.9f));

        //������Կ�ָ����չ���
        Basement exteBas = new Basement(100.0f);
        Debug.Log(exteBas.BeReferredObjectsValues);
        {
            Modifier localModi = new Modifier(new Memoless(50.0f), exteBas, ModifyOperations.ADD);
            Modifier localModi2 = new Modifier(localModi, exteBas, ModifyOperations.ADD);
            ; Debug.Log(exteBas.BeReferredObjectsValues);
            exteBas.Value = 150.0f;
            Debug.Log(localModi2.Value);
        }
        GC.Collect();
        exteBas.NeedReCalculate();
        exteBas.Value = 0;
        for(int i=0;i<1024;i++)
        {
            Modifier modi=new Modifier(new Memoless(50.0f), exteBas, ModifyOperations.ADD);
        }
        GC.Collect();
        //Debug.Log(exteBas.BeReferredObjectsValues);
        exteBas.Value = 0;
        Debug.Log(exteBas.BeReferredObjectsValues);
    }

    [Test]
    public void HealthConstruction()
    {
        FloatEqualityComparer fq = new FloatEqualityComparer(10e-8f);
        //������������
        Basement base_health = new Basement(100.0f);
        //��ҵȼ�
        Basement player_level = new Basement(15.0f);
        //ÿ�ȼ����ӵ�����ֵ�ٷֱ�
        Basement health_bonus_per_level = new Basement(0.02f);
        //��Դ�������õ����������޼ӳ�
        Modifier upgradeParam = new Modifier(player_level, health_bonus_per_level, ModifyOperations.MULTIPLY);
        Modifier healthBonusFromUpgrade = new Modifier(base_health, upgradeParam, ModifyOperations.MULTIPLY);
        //��������ֵ
        Modifier finalHealth = new Modifier(base_health, healthBonusFromUpgrade, ModifyOperations.ADD);
        Debug.Log(finalHealth.Value);
        Assert.IsTrue(fq.Equals(finalHealth.Value, 130.0f));

        base_health.Value += 10.0f;
        Debug.Log("������10���������ֵ");
        Debug.Log(finalHealth.Value);
        Assert.IsTrue(fq.Equals(finalHealth.Value, 143.0f));

        player_level.Value += 10.0f;
        Debug.Log("������10����ҵȼ�");
        Debug.Log(finalHealth.Value);
        Assert.IsTrue(fq.Equals(finalHealth.Value, 165.0f));

        health_bonus_per_level.Value += 0.01f;
        Debug.Log("������1��ÿ�ȼ�����ֵ�ӳɰٷֱ�");
        Debug.Log(finalHealth.Value);
        Assert.IsTrue(fq.Equals(finalHealth.Value, 192.5f));

        //����ʹ����������غ������������������
        Debug.Log("ʹ�������������!");
        //������������
        Basement base_health2 = new Basement(100.0f);
        //��ҵȼ�
        Basement player_level2 = new Basement(15.0f);
        //ÿ�ȼ����ӵ�����ֵ�ٷֱ�
        Basement health_bonus_per_level2 = new Basement(0.02f);

        Modifier finalHealth2 = base_health2 + (base_health2 * (player_level2 * health_bonus_per_level2));
        Debug.Log(finalHealth2.Value);
        Assert.IsTrue(fq.Equals(finalHealth2.Value, 130.0f));

        base_health2.Value += 10.0f;
        Debug.Log("������10���������ֵ");
        Debug.Log(finalHealth2.Value);
        Assert.IsTrue(fq.Equals(finalHealth2.Value, 143.0f));

        player_level2.Value += 10.0f;
        Debug.Log("������10����ҵȼ�");
        Debug.Log(finalHealth2.Value);
        Assert.IsTrue(fq.Equals(finalHealth2.Value, 165.0f));

        health_bonus_per_level2.Value += 0.01f;
        Debug.Log("������1��ÿ�ȼ�����ֵ�ӳɰٷֱ�");
        Debug.Log(finalHealth2.Value);
        Assert.IsTrue(fq.Equals(finalHealth2.Value, 192.5f));

        //����������ֵΪԭֵ
        base_health.Value = 100.0f;
        player_level.Value=15.0f;
        health_bonus_per_level.Value = 0.02f;
        Debug.Log(finalHealth.Value);
        Assert.IsTrue(fq.Equals(finalHealth.Value, 130.0f));

        base_health2.Value = 100.0f;
        player_level2.Value = 15.0f;
        health_bonus_per_level2.Value = 0.02f;
        Debug.Log(finalHealth2.Value);
        Assert.IsTrue(fq.Equals(finalHealth2.Value, 130.0f));
    }

    [Test]
    public void GameValueSpecialTestPasses()
    {
        FloatEqualityComparer fq = new FloatEqualityComparer(10e-8f);
        //���Խṹ������������
        {
            Memoless mm = new Memoless(22);
            Basement bs = new Basement(25);
            Modifier sum1 = bs+mm;
            Modifier sum2 = mm+bs;
            Assert.IsTrue(fq.Equals(sum1.Value, 47.0f));
            Assert.IsTrue(fq.Equals(sum2.Value, 47.0f));
        }
        //�Բ��ɸ�ֵ��Modifier��ֵ
        {
            Modifier sum = (Memoless)23f + (Memoless)24;
            Assert.IsTrue(fq.Equals(sum.Value, 47.0f));
            bool excHapp = false;
            try
            {
                sum.Value = 4;
            }
            catch(NotImplementedException)
            {
                excHapp = true;
            }
            Assert.IsTrue(excHapp);
        }
        //��������
        {
            Basement bs1 = new Basement(1.0f);
            Basement bs2 = new Basement(2.0f);
            Modifier md = bs1 - bs2;
            Assert.IsTrue(fq.Equals(md.Value, -1.0f));
            md.Swap();
            Assert.IsTrue(fq.Equals(md.Value, 1.0f));
        }
    }

    [Test]
    public void SelfPointerTestPasses()
    {
        Basement base_hp = new Basement(100.0f);
        Basement add_hp = new Basement(15.0f);
        Modifier modi = add_hp+ base_hp;
        Debug.Log(modi.Value);
        bool exhp = false;
        try
        {
            modi.SetPartnerValue(modi);
            Debug.Log(modi.Value);
        }
        catch(StackOverflowException e)
        {
            exhp = true;
            Debug.Log(e.ToString());
        }
        Assert.IsTrue(exhp);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}

